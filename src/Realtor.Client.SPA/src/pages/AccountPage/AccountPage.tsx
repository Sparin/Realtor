import React from 'react';
import { Form } from 'antd';
import LoginForm from '../../components/LoginForm/LoginForm';

export default class LoginPage extends React.Component<any, any> {

    render() {
        const WrappedForm = Form.create()(LoginForm);
        return (<WrappedForm />);
    }
}