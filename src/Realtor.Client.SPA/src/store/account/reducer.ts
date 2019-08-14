import * as Actions from './actions'

const initialState = {
    username: undefined,
    isLoading: false
}

export const reducer = (state = initialState, action: any) => {
    switch (action.type) {

        // Login
        case Actions.ACCOUNT_LOGIN_REQUEST:
            return { ...state, isLoading: true };
        case Actions.ACCOUNT_LOGIN_RECEIVE:
            return { ...state, username: action.value, isLoading: false };
        case Actions.ACCOUNT_LOGIN_FAILED:
            return { ...state, isLoading: false };

        // Register
        case Actions.ACCOUNT_REGISTER_REQUEST:
            return { ...state, isLoading: true };
        case Actions.ACCOUNT_REGISTER_RECEIVE:
            return { ...state, username: action.value, isLoading: false };
        case Actions.ACCOUNT_REGISTER_FAILED:
            return { ...state, isLoading: false };

        // Logout
        case Actions.ACCOUNT_LOGOUT_REQUEST:
            return { ...state, isLoading: true };
        case Actions.ACCOUNT_LOGOUT_RECEIVE:
            return { ...state, username: undefined, isLoading: false };
        case Actions.ACCOUNT_LOGOUT_FAILED:
            return { ...state, isLoading: false };
    }

    return state;
};