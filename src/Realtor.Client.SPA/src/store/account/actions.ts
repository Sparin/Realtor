import { Dispatch } from 'redux'
import * as api from '../../api/account/methods'

// Account
export const ACCOUNT_LOGIN_REQUEST = 'ACCOUNT_LOGIN_REQUEST';
export const ACCOUNT_LOGIN_RECEIVE = 'ACCOUNT_LOGIN_RECEIVE';
export const ACCOUNT_LOGIN_FAILED = 'ACCOUNT_LOGIN_FAILED';

export const ACCOUNT_REGISTER_REQUEST = 'ACCOUNT_REGISTER_REQUEST';
export const ACCOUNT_REGISTER_RECEIVE = 'ACCOUNT_REGISTER_RECEIVE';
export const ACCOUNT_REGISTER_FAILED = 'ACCOUNT_REGISTER_FAILED';

export const ACCOUNT_LOGOUT_REQUEST = 'ACCOUNT_LOGOUT_REQUEST';
export const ACCOUNT_LOGOUT_RECEIVE = 'ACCOUNT_LOGOUT_RECEIVE';
export const ACCOUNT_LOGOUT_FAILED = 'ACCOUNT_LOGOUT_FAILED';

export const Actions = {
    login: (username: string, password: string, rememberLogin: boolean) => (dispatch: Dispatch, getState: Function) => {
        if (getState().user.isLoading)
            return;
        dispatch({ type: ACCOUNT_LOGIN_REQUEST });

        api.login(username, password, rememberLogin)
            .then(() => {
                dispatch({ type: ACCOUNT_LOGIN_RECEIVE, value: username })
            })
            .catch(error => {
                dispatch({ type: ACCOUNT_LOGIN_FAILED })
            });
    },

    register: (username: string, password: string, confirmPassword: string) => (dispatch: Dispatch, getState: Function) => {
        if (getState().user.isLoading)
            return;
        dispatch({ type: ACCOUNT_REGISTER_REQUEST });

        api.register(username, password, confirmPassword)
            .then(() => {
                dispatch({ type: ACCOUNT_REGISTER_RECEIVE, value: username })
            })
            .catch(error => {
                dispatch({ type: ACCOUNT_REGISTER_FAILED })
            });
    },

    logout: (username: string, password: string, confirmPassword: string) => (dispatch: Dispatch, getState: Function) => {
        if (getState().account.isLoading)
            return;
        dispatch({ type: ACCOUNT_LOGOUT_REQUEST });

        api.register(username, password, confirmPassword)
            .then(() => {
                dispatch({ type: ACCOUNT_LOGOUT_RECEIVE })
            })
            .catch(error => {
                dispatch({ type: ACCOUNT_LOGOUT_FAILED })
            });
    },
}