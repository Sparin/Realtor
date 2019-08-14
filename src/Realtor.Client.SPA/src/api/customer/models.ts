export interface Customer {
    id: number
    username: string
    phones: Array<Phone>
}
export interface Phone {
    number: string
}