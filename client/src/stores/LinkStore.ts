import {makeAutoObservable, runInAction} from 'mobx'
import ILinkService, {GetListResponse} from '../services/LinkService/interfaces/ILinkService.ts';
import Link from '../services/LinkService/interfaces/Link.ts';
import Tag from '../services/LinkService/interfaces/Tag.ts';
import PreviewLink from '../services/LinkService/interfaces/PreviewLink.ts';
import {GetListRequest} from '../services/LinkService/interfaces/ILinkService.ts';

type Filter = {
    tags: string[];
    title: string;
    liked: boolean;
    saved: boolean;
    owned: boolean;
};

type Preview = {
    url?: string;
    link?: PreviewLink;
};

type LinkStoreState = {
    isListLoading: boolean;
    links: Link[];
    tags: Tag[];
    filter: Filter;
    preview: Preview;
    paginationState: Pick<GetListResponse, 'totalPages' | 'totalCount' | 'hasNextPage' | 'hasPreviousPage'>;
};

class LinkStore {
    constructor(private readonly linkService: ILinkService) {
        makeAutoObservable(this);

        this.getList();
    }

    state: LinkStoreState = {
        isListLoading: true,
        links: [],
        tags: [],
        filter: {
            tags: [],
            title: '',
            liked: false,
            saved: false,
            owned: false,
        },
        preview: {
        },
        paginationState: {
            totalPages: 0,
            totalCount: 0,
            hasPreviousPage: false,
            hasNextPage: false,
        }
    };

    private paginationParams: Pick<GetListRequest, 'pageSize' | 'pageNumber'> = {
        pageNumber: 1,
        pageSize: 10
    };

    public get sortedTags() {
        return this.state.tags.slice().sort((a, b) => b.count - a.count);
    }

    public getList = async () => {
        runInAction(() => {
            this.state.isListLoading = true;
        });
        this.paginationParams.pageNumber = 1;
        const response = await this.linkService.getList({
            ...this.paginationParams,
            ...this.state.filter,
        });
        this.paginationParams.pageNumber = this.paginationParams.pageNumber + 1;
        runInAction(() => {
            this.state = {
                ...this.state,
                links: response.items,
                tags: [...new Set<Tag>(response.tags)],
                isListLoading: false,
                paginationState: {
                    totalPages: response.totalPages,
                    totalCount: response.totalCount,
                    hasPreviousPage: response.hasPreviousPage,
                    hasNextPage: response.hasNextPage,
                }
            };
        });
    };

    public loadMore = async () => {
        console.log('loadMore')
        if (this.state.paginationState.hasNextPage) {
            console.log('loadMore2')
            const response = await this.linkService.getList({
                ...this.paginationParams,
                ...this.state.filter,
            });
            this.paginationParams.pageNumber = this.paginationParams.pageNumber + 1;
            runInAction(() => {
                this.state.links = [...this.state.links, ...response.items];
                this.state.paginationState = {
                    totalPages: response.totalPages,
                    totalCount: response.totalCount,
                    hasPreviousPage: response.hasPreviousPage,
                    hasNextPage: response.hasNextPage,
                }

            });
        }
    }

    public toggleTagFilter = async (tagName: string) => {
        const tag = this.state.tags.find(tag => tag.name === tagName);
        if (!tag) {
            throw new Error('Tag not found');
        }

        if (this.state.filter.tags.find(t => t === tag.name)) {
            this.state.filter.tags = this.state.filter.tags.filter(t => t !== tagName);
        } else {
            this.state.filter.tags = [...this.state.filter.tags, tag.name];
        }

        await this.getList();
    };

    public toggleLikedFilter = async () => {
        this.state.filter.liked = !this.state.filter.liked;
        try {
            await this.getList();
        } catch (e) {
            runInAction(() => {
                this.state.filter.liked = !this.state.filter.liked;
            })
        }
    }

    public toggleSavedFilter = async () => {
        this.state.filter.saved = !this.state.filter.saved;
        try {
            await this.getList();
        } catch (e) {
            runInAction(() => {
                this.state.filter.saved = !this.state.filter.saved;
            })
        }
    }

    public toggleOwnedFilter = async () => {
        this.state.filter.owned = !this.state.filter.owned;
        try {
            await this.getList();
        } catch (e) {
            runInAction(() => {
                this.state.filter.owned = !this.state.filter.owned;
            })
        }
    }

    public setFilterTitle = (title: string) => {
        this.state.filter.title = title;
    };

    public likeLink = async (id: string) => {
        const link = this.state.links.find(link => link.id === id);
        if (!link) {
            throw new Error('Link not found');
        }

        await this.linkService.like({ linkId: id, state: !link.isLiked });

        const index = this.state.links.findIndex(link => link.id === id);
        this.state.links[index] = {
            ...link,
            isLiked: !link.isLiked,
            likes: link.isLiked ? link.likes - 1 : link.likes + 1
        };
    };

    public saveLink = async (id: string) => {
        const link = this.state.links.find(link => link.id === id);
        if (!link) {
            throw new Error('Link not found');
        }

        await this.linkService.save({ linkId: id, state: !link.isSaved });

        const index = this.state.links.findIndex(link => link.id === id);
        this.state.links[index] = {
            ...link,
            isSaved: !link.isSaved
        };
    };

    public previewLink = async (url: string): Promise<{errorMessage?: string }> => {
        this.state.preview = {
            link: undefined,
        };

        const previewLink = await this.linkService.previewLink({ url });

        runInAction(() => {
            this.state.preview = {
                url,
                link: previewLink
            };
        });

        return {};
    };

    public updatePreviewLink = (updates: Partial<PreviewLink>) => {
        if (!this.state.preview.link) {
            throw new Error('Preview Link not found');
        }

        this.state.preview.link = {
            ...this.state.preview.link,
            ...updates,
        };
    };

    public submitLink = async (): Promise<void> => {
        if (!this.state.preview.link) {
            throw new Error('Preview Link not found');
        }

        const link = await this.linkService.addLink({
            url: this.state.preview.url!,
            ...this.state.preview.link,
        });

        const tagsToAdd = link.tags.filter(tag => !this.state.tags.some(t => t.name === tag));
        const tags = [
            ...this.state.tags.map(tag => {
                if (link.tags.includes(tag.name)) {
                    return {
                        ...tag,
                        count: tag.count + 1
                    };
                }

                return tag;
            }),
            ...tagsToAdd.map(tag => ({ name: tag, count: 1 }))
        ];
        runInAction(() => {
            this.state.links = [link, ...this.state.links];
            this.state.preview = {};
            this.state.tags = tags;
        });
    };

    public deleteLink = async (id: string): Promise<void> => {
        const link = this.state.links.find(link => link.id === id);
        if (!link) {
            throw new Error('Link not found');
        }

        await this.linkService.delete({ linkId: id });

        const tagsToRemove = this.state.tags.filter(tag => link.tags.includes(tag.name) && tag.count === 1);
        runInAction(() => {
            this.state.links = this.state.links.filter(link => link.id !== id);
            this.state.tags = this.state.tags.filter(tag => !tagsToRemove.includes(tag));
            this.state.filter.tags = this.state.filter.tags.filter(tagName => !tagsToRemove.some(tag => tag.name === tagName));
        });
    };
}

export default LinkStore;