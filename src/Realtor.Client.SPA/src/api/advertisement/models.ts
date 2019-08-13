export interface Advertisement {
    id: number
    authorId: number
    shortDescription:string
    roomCount: number
    price: number
}

export enum AdvertisementType{
    Ask = "Ask",
    Offer = "Offer"
}
export interface SearchResponse<T> {
    totalItems: number
    totalPages: number
    currentPage: number
    items: Array<T>
}

export interface SearchOptions {
    type: AdvertisementType
    minimumRooms: number
    maximumRooms: number
    minimumPrice: number
    maximumPrice: number
}