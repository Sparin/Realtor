import React from 'react';
import './App.css';
import { Route, Switch, Redirect } from 'react-router';
import Layout from './pages/Layout/Layout';
import NotFound from './pages/NotFound/NotFound';
import Advertisements from './pages/Advertisements/Advertisements';
import AccountPage from './pages/AccountPage/AccountPage';
import Advertisement from '../src/components/Advertisement/Advertisement';
import Customer from '../src/components/Customer/Customer';

const App: React.FC = () => {
  return (
    <div className="App">
      <Layout>
        <Switch>
          <Route path="/" exact component={Advertisements} />
          <Route path="/advertisements" exact component={Advertisements} />
          <Route path="/advertisement/:id(\d+)?/:action(edit|create)?" exact component={Advertisement} />
          <Route path="/customer/:id(\d+)?/:action(edit|create)?" exact component={Customer} />
          <Route path="/account/:action(login|register)" exact component={AccountPage} />
          <Route path="/404" component={NotFound} />
          <Redirect from='*' to='/404' />
        </Switch>
      </Layout>
    </div>
  );
}

export default App;
