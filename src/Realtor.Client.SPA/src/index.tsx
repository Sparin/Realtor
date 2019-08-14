import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';
import { createBrowserHistory, BrowserHistoryBuildOptions } from 'history';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { ConnectedRouter } from 'connected-react-router';
import { StoreBuilder as Store } from './store/StoreBuilder';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const historyOptions: BrowserHistoryBuildOptions = {
    basename: baseUrl as string
};
const history = createBrowserHistory(historyOptions);

// Get the application-wide store instance, prepopulating with state from the server where available.
const store = Store.configureStore(history)

const app = (
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </ConnectedRouter>
    </Provider>
);

ReactDOM.render(app, document.getElementById('root'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
