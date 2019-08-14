import React from 'react';
import { Descriptions, Button, Modal, List } from 'antd';
import { withRouter, Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { Phone } from '../../api/customer/models';

const { confirm } = Modal;

class ViewForm extends React.Component<any, any> {
    showDeleteConfirm = () => {
        const deleteFunc = this.props.onDelete;
        confirm({
            title: 'Are you sure delete this advertisement?',
            okText: 'Yes',
            okType: 'danger',
            cancelText: 'No',
            onOk() {
                deleteFunc();
            },
        });
    }

    render() {
        return (
            <div>
                <Descriptions bordered column={1} style={{ marginBottom: "16px", width: "600px" }}>
                    <Descriptions.Item label="Author">{this.props.customer.username}</Descriptions.Item>
                    <Descriptions.Item label="Phones">
                        <List
                            size="small"
                            dataSource={this.props.customer.phones}
                            renderItem={(phone: Phone) => <List.Item>{phone.number}</List.Item>}
                        />
                    </Descriptions.Item>
                    <Descriptions.Item label="Type">{this.props.advertisement.type}</Descriptions.Item>
                    <Descriptions.Item label="Short description">{this.props.advertisement.shortDescription}</Descriptions.Item>
                    <Descriptions.Item label="Rooms">{this.props.advertisement.roomsCount}</Descriptions.Item>
                    <Descriptions.Item label="Price">{this.props.advertisement.price}</Descriptions.Item>
                </Descriptions>
                {this.props.account.username === this.props.customer.username ?
                    <div style={{
                        position: "relative",
                        float: "right"
                    }}>
                        <Link to={`/advertisement/${this.props.advertisement.id}/edit`} >
                            <Button style={{ marginRight: "8px" }}>Edit</Button>
                        </Link>
                        <Button onClick={this.showDeleteConfirm} type="danger">Delete</Button>
                    </div>
                    : <div></div>}
            </div>
        );
    }
}

const mapStateToProps = (state: any) => {
    return {
        account: state.account
    }
};

export default connect(mapStateToProps)(withRouter(ViewForm));