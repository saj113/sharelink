import {useStore} from '../../../../contexts/AppContext.tsx';
import LinkStore from '../../../../stores/LinkStore.ts';
import {observer} from 'mobx-react-lite';
import TagBadge from '../../../../components/TagBadge.tsx';
import {formatTagTitle} from '../../../../utils/format.ts';
import Skeleton from '../../../../components/Skeleton.tsx';
import {useMemo} from 'react';

const MaxTagsShown = 12;

const TagFilter = observer(() => {
    const { state: { tags, filter: { tags: appliedTags }, isListLoading }, applyTagFilter, removeTagFilter } = useStore<LinkStore>(LinkStore);

    const sortedTags = useMemo(() => tags.slice().sort((a, b) => b.count - a.count), [tags]);
    const showSkeletons = isListLoading && tags.length === 0;
    return (
        <>
            { showSkeletons && (
                <div className="flex flex-col justify-center items-center w-full gap-2">
                    <Skeleton className="w-full" />
                    <Skeleton className="w-full" />
                </div>
            ) || <div className="flex flex-row flex-wrap justify-center gap-2 items-center">
                { appliedTags.map((tag) => (
                    <button key={tag} onClick={() => removeTagFilter(tag)}>
                        <TagBadge title={tag} removable />
                    </button>
                ))}
                { sortedTags.filter(x => !appliedTags.some(e => e === x.name)).slice(0, MaxTagsShown).map((tag) => (
                    <div key={tag.name} onClick={() => applyTagFilter(tag.name)} className="flex flex-row items-center justify-center py-1 text-sm mr-2 font-medium cursor-pointer secondary-text-color">
                        <span className="mr-1">{formatTagTitle(tag.name)}</span>
                        <span className="text-xs font-normal text-gray-500 dark:text-gray-400 dark:bg-zinc-700 bg-gray-100 rounded-full px-1">{tag.count}</span>
                    </div>
                )) }
            </div>}
        </>
    );
});

export default TagFilter;