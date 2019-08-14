import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { connectRouter, routerMiddleware } from 'connected-react-router'
import * as Account from './account/reducer';

export class StoreBuilder {
    public static configureStore(history: any) {
        const reducers = {      
            account: Account.reducer
        };

        const middleware = [
            thunk,
            routerMiddleware(history)
        ];

        // In development, use the browser's Redux dev tools extension if installed
        const enhancers = [];
        const isDevelopment = process.env.NODE_ENV === 'development';
        if (isDevelopment && typeof window !== 'undefined' && (window as any).__REDUX_DEVTOOLS_EXTENSION__) {
            enhancers.push((window as any).__REDUX_DEVTOOLS_EXTENSION__());
        }

        const rootReducer = combineReducers({
            ...reducers,
            router: connectRouter(history),
        });

        return createStore(
            rootReducer,
            compose(applyMiddleware(...middleware), ...enhancers)
        );
    }
}