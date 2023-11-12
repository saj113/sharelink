import ILinkService, {
    AddLinkRequest,
    GetListResponse, LikeLinkRequest, PreviewLinkRequest, PreviewLinkResponse, SaveLinkRequest
} from './interfaces/ILinkService.ts';
import HttpClient from '../HttpClient/HttpClient.ts';
import Link from './interfaces/Link.ts';

class LinkService implements ILinkService {

    private readonly baseUrl = `${import.meta.env.VITE_SHARELINK_API_BASE_URL}/api/links`;

    constructor(private readonly httpClient: HttpClient) {
    }

    async getList(): Promise<GetListResponse> {
        const response = await this.httpClient.get<GetListResponse>(`${this.baseUrl}/list`);

        return response.data;
    }

    async addLink(request: AddLinkRequest): Promise<Link> {
        const response = await this.httpClient.post<AddLinkRequest, Link>(
            `${this.baseUrl}/create`, { ...request });

        return response.data;
    }

    async previewLink(request: PreviewLinkRequest): Promise<PreviewLinkResponse> {
        const response = await this.httpClient.post<PreviewLinkRequest, PreviewLinkResponse>(
            `${this.baseUrl}/preview`, { ...request });

        return response.data;
    }

    async like(request: LikeLinkRequest): Promise<void> {
        await this.httpClient.post<LikeLinkRequest, {}>(`${this.baseUrl}/like`, { ...request });
    }

    async save(request: SaveLinkRequest): Promise<void> {
        await this.httpClient.post<SaveLinkRequest, {}>(`${this.baseUrl}/save`, { ...request });
    }

    // @ts-ignore
    remove(linkId: string): Promise<void> {
        return Promise.resolve();
    }
}

export default LinkService;