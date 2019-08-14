import React from 'react';
import { getAdvertisement, deleteAdvertisement, createAdvertisement, updateAdvertisement } from '../../api/advertisement/methods';
import * as Models from '../../api/advertisement/models';
import { PageHeader, Form, Spin } from 'antd';
import ViewForm from './ViewForm';
import EditForm from './EditForm';
import { getCustomer } from '../../api/customer/methods';
import { connect } from 'react-redux';

class Advertisement extends React.Component<any, any> {

    constructor(props: any) {
        super(props);

        let action = "view";
        let id = this.props.match.params.id;
        if (this.props.match.params.action)
            action = this.props.match.params.action;
        this.state = { id, action, advertisement: {}, customer: {}, isLoading: true, errors: undefined };
        this.onSubmit = this.onSubmit.bind(this);
        this.onDelete = this.onDelete.bind(this);
    }

    componentDidMount() {
        if (this.props.match.params.action === 'create')
            this.setState({ isLoading: false });
        else
            getAdvertisement(this.state.id)
                .then(advertisement => {
                    getCustomer(advertisement.authorId)
                        .then(customer => {
                            this.setState({ advertisement, customer, isLoading: false });
                        })
                        .catch(err => {
                            this.props.history.push('/404')
                        });;

                })
                .catch(err => {
                    this.props.history.push('/404')
                });
    }

    onSubmit(advertisement: Models.Advertisement) {
        this.setState({ isLoading: true });
        let request;
        switch (this.props.match.params.action) {
            case "create": request = createAdvertisement(advertisement); break;
            case "edit": request = updateAdvertisement(advertisement.id, advertisement); break;
            default: throw Error("Unpredictable action on advertisement submit");
        }

        request.then(advertisement => {
            if (!this.state.customer.id)
                getCustomer(advertisement.authorId)
                    .then(customer => {
                        this.setState({ advertisement, customer, isLoading: false });
                    })
            this.setState({ advertisement, isLoading: false });
            this.props.history.push(`/advertisement/${advertisement.id}`)
        }).catch(err => {

        });
    }

    async onDelete() {
        this.setState({ isLoading: true });
        await deleteAdvertisement(this.props.match.params.id);
        this.props.history.push('/');
    }

    render() {
        if (this.state.isLoading)
            return (<Spin tip="Loading..." style={{ width: "100%", margin: "16px 0" }} />);
        let form;
        const WrappedEditForm = Form.create<any>()(EditForm);
        switch (this.props.match.params.action) {
            case "create":
            case "edit": form = <WrappedEditForm advertisement={this.state.advertisement} action={this.props.match.params.action} onSubmit={this.onSubmit} errors={this.state.errors} />; break;
            default: form = (<ViewForm advertisement={this.state.advertisement} customer={this.state.customer} onDelete={this.onDelete} />); break;
        }

        return (<div>
            <PageHeader onBack={() => this.props.history.goBack()} title={this.props.match.params.action !== 'create' ? `Advertisement #${this.state.advertisement.id}` : 'Create new advertisement'} />
            <div style={{ margin: "16px", display: "flex", justifyContent: "center" }}>
                {form}
            </div>
        </div>)
    }
}

const mapStateToProps = (state: any) => {
    return {
        account: state.account
    }
};

export default connect(mapStateToProps)(Advertisement);