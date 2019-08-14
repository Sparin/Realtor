
import makeHttpRequest from '../makeHttpRequest';


export function login(username: string, password: string, rememberLogin: boolean): Promise<void> {
    const body = { username, password, rememberLogin };
    const url = `/api/account/login`;

    return makeHttpRequest<void>(url, 'POST', body);
}

export function register(username: string, password: string, confirmPassword: string): Promise<void> {
    const body = { username, password, confirmPassword };
    const url = `/api/account/register`;

    return makeHttpRequest<void>(url, 'POST', body);
}

export function logout(): Promise<void> {
    const url = `/api/account/logout`;

    return makeHttpRequest<void>(url, 'POST');
}