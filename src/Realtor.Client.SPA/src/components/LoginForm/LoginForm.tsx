import React from 'react';
import { Form, Icon, Input, Button, Checkbox, notification } from 'antd';
import './LoginForm.css'
import { withRouter } from 'react-router';
import { Actions } from '../../store/account/actions';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';

class LoginForm extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        this.state = {
            username: "",
            password: "",
            rememberLogin: true
        }
    }

    handleSubmit = (e: any) => {
        e.preventDefault();
        this.props.form.validateFields((err: any, form: any) => {
            if (!err)
                this.props.login(form.username, form.password, form.rememberLogin);
        });
    }

    componentDidUpdate(prevProps: any, prevState: any) {
        if (prevProps.account.isLoading === true && this.props.account.isLoading === false)
            if (prevProps.account.username !== this.props.account.username)
                this.props.history.push('/');
            else
                notification['error']({
                    message: 'Authentification failed',
                    description: 'Invalid username or password'
                });
    }

    render() {
        const { getFieldDecorator } = this.props.form;

        return (
            <div style={{ margin: "16px", display: "flex", justifyContent: "center" }}>
                <Form onSubmit={this.handleSubmit} className="login-form">
                    <Form.Item>
                        {getFieldDecorator('username', {
                            rules: [{ required: true, message: 'Please input your Username!' }],
                        })(
                            <Input
                                prefix={<Icon type="user" style={{ color: 'rgba(0,0,0,.25)' }} />}
                                placeholder="Username"
                            />,
                        )}
                    </Form.Item>
                    <Form.Item>
                        {getFieldDecorator('password', {
                            rules: [{ required: true, message: 'Please input your Password!' }],
                        })(
                            <Input
                                prefix={<Icon type="lock" style={{ color: 'rgba(0,0,0,.25)' }} />}
                                type="password"
                                placeholder="Password"
                            />,
                        )}
                    </Form.Item>
                    <Form.Item>
                        {getFieldDecorator('remember', {
                            valuePropName: 'checked',
                            initialValue: this.state.rememberLogin,
                        })(<Checkbox >Remember me</Checkbox>)}
                        <Button type="primary" htmlType="submit" className="login-form-button">
                            Log in
                        </Button>
                        {/* Or <a href="/account/register">register now!</a> */}
                    </Form.Item>
                </Form></div>);


    }
}

const mapStateToProps = (state: any) => {
    return {
        account: state.account
    }
};

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        login: (username: string, password: string, rememberLogin: boolean) =>
            dispatch(Actions.login(username, password, rememberLogin) as any)
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(LoginForm));