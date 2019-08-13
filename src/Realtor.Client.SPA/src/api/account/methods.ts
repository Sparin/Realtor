import { RegistrationForm, LoginForm } from './models';
import makeHttpRequest from '../makeHttpRequest';


export function login(form: LoginForm): Promise<void> {
    const body = form;
    const url = `/api/account/login`;

    return makeHttpRequest<void>(url, 'POST', body);
}

export function register(form: RegistrationForm): Promise<void> {
    const body = form;
    const url = `/api/account/register`;

    return makeHttpRequest<void>(url, 'POST', body);
}

export function logout(form: RegistrationForm): Promise<void> {
    const body = form;
    const url = `/api/account/logout`;

    return makeHttpRequest<void>(url, 'POST', body);
}