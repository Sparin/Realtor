import { Phone, Customer } from './models';
import makeHttpRequest from '../makeHttpRequest';

export function getCustomer(id: number): Promise<Customer> {
    const url = `/api/customer/${id}`;

    return makeHttpRequest<Customer>(url, 'GET');
}

export function getPhones(customerId: number): Promise<Array<Phone>> {
    const url = `/api/customer/${customerId}/phones`;

    return makeHttpRequest<Array<Phone>>(url, 'GET');
}

export function getMyPhones(): Promise<Array<Phone>> {
    const url = `/api/customer/phones`;

    return makeHttpRequest<Array<Phone>>(url, 'GET');
}

export function createPhone(phone: Phone): Promise<Phone> {
    const body = phone
    const url = `/api/customer/phones`;

    return makeHttpRequest<Phone>(url, 'POST', body);
}

export function updatePhone(id: number, phone: Phone): Promise<Phone> {
    const body = phone
    const url = `/api/customer/phones/${id}`;

    return makeHttpRequest<Phone>(url, 'PUT', body);
}

export function deletePhone(id: number) {
    const url = `/api/customer/phones/${id}`;

    return makeHttpRequest<any>(url, 'DELETE');
}