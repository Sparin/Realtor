import React from 'react';
import { Form, Input, Radio, Button, InputNumber } from 'antd';
import { withRouter } from 'react-router-dom';
import { AdvertisementType } from '../../api/advertisement/models';

class EditForm extends React.Component<any, any> {

    types = Object.keys(AdvertisementType).filter((value, index, self) => self.indexOf(value) === index);

    constructor(props: any) {
        super(props);

        this.state = { ...this.props.advertisement };
    }

    onSubmit = (event: any) => {
        if (event)
            event.preventDefault();
        this.props.form.validateFields((err: any) => {
            if (!err) {
                this.props.onSubmit({ ...this.props.form.getFieldsValue(), id: this.state.id });
            }
        })
    };

    render() {
        const { getFieldDecorator } = this.props.form;

        return (
            <Form layout="vertical" onSubmit={this.onSubmit} style={{ width: "300px" }}>
                <Form.Item label="Type" >
                    {getFieldDecorator('type', {
                        initialValue: this.state.type,
                        rules: [{ required: true, message: 'Please select the type of advertisement' }],
                    })(
                        <Radio.Group onChange={e => this.props.form.setFieldsValue({ type: e.target.value })}>
                            {this.types.map(type => <Radio value={type} key={type}>{type}</Radio>)}
                        </Radio.Group>)}
                </Form.Item>

                <Form.Item label="Short description">
                    {getFieldDecorator('shortDescription', {
                        initialValue: this.state.shortDescription,
                        rules: [
                            { required: true, message: 'Please input short description of advertisement' },
                            { max: 100, message: 'Maximum 100 symbols' }
                        ],
                    })(<Input onChange={e => this.props.form.setFieldsValue({ shortDescription: e.target.value })} />)}
                </Form.Item>

                <Form.Item label="Rooms" >
                    {getFieldDecorator('roomsCount', {
                        initialValue: this.state.roomsCount,
                        rules: [
                            { required: true, message: 'Please tell how many rooms at your apartment' },
                            {
                                validator: (rule: any, value: number, callback: any) => value <= 0 || value > 100 ? callback('Value for Rooms must be between 1 and 100.') : callback(),
                                message: 'Value for Rooms must be between 1 and 100.'
                            }],
                    })(<InputNumber style={{ width: "100%" }} min={0} onChange={value => this.props.form.setFieldsValue({ roomsCount: value })} />)}
                </Form.Item>

                <Form.Item label="Price" >
                    {getFieldDecorator('price', {
                        initialValue: this.state.price,
                        rules: [
                            { required: true, message: 'Please input price' },
                            {
                                validator: (rule: any, value: number, callback: any) => value <= 0 ? callback('The price should be a positive number') : callback(),
                                message: 'The price should be a positive number'
                            }],
                    })(<InputNumber style={{ width: "100%" }} min={0} onChange={value => this.props.form.setFieldsValue({ price: value })} />)}
                </Form.Item>

                <Form.Item >
                    <Button style={{ width: "100%" }} type="primary" onClick={this.onSubmit}>
                        Submit
                    </Button>
                </Form.Item>
            </Form>
        )
    }
}

export default withRouter(EditForm);