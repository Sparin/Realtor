import { SearchOptions, Advertisement, SearchResponse } from './models';
import makeHttpRequest, { defaultPage, defaultLimit } from '../makeHttpRequest';
import queryString from 'query-string';

export function searchAdvertisements(
    page: number = defaultPage,
    limit: number = defaultLimit,
    options: SearchOptions
) {
    const params = { ...options, page, limit };
    const url = `/api/advertisement?` + queryString.stringify(params);

    return makeHttpRequest<SearchResponse<Advertisement>>(url, 'GET');
}

export function getAdvertisement(id: number): Promise<Advertisement> {
    const url = `/api/advertisement/${id}`;

    return makeHttpRequest<Advertisement>(url, 'GET');
}

export function createAdvertisement(advertisement: Advertisement): Promise<Advertisement> {
    const body = advertisement
    const url = `/api/advertisement`;

    return makeHttpRequest<Advertisement>(url, 'POST', body);
}

export function updateAdvertisement(id: number, advertisement: Advertisement): Promise<Advertisement> {
    const body = advertisement
    const url = `/api/advertisement/${id}`;

    return makeHttpRequest<Advertisement>(url, 'PUT', body);
}

export function deleteAdvertisement(id: number) {
    const url = `/api/advertisement/${id}`;

    return makeHttpRequest<any>(url, 'DELETE');
}