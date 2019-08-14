import React from 'react';
import { Form, Button, Slider, Radio } from 'antd';
import { SearchOptions, AdvertisementType } from '../../api/advertisement/models';


export default class SearchFilters extends React.Component<any, any> {

    adTypes = Object.keys(AdvertisementType).filter((value, index, self) => self.indexOf(value) === index);
    constructor(props: any) {
        super(props);

        this.state = { minimumRooms: 1, maximumRooms: 100, minimumPrice: 1, maximumPrice: 1000000 } as SearchOptions;
    }

    onSubmit = (event: any) => {
        if (event)
            event.preventDefault();
        this.props.onApply(this.state);
    }

    render() {
        return (
            <Form layout="vertical" onSubmit={this.onSubmit}>
                <Form.Item label="Rooms">
                    <Slider
                        range
                        value={[this.state.minimumRooms, this.state.maximumRooms]}
                        max={100}
                        min={1}
                        onChange={(values: any) => this.setState({ minimumRooms: values[0], maximumRooms: values[1] })} />
                </Form.Item>

                <Form.Item label="Price">
                    <Slider
                        range
                        value={[this.state.minimumPrice, this.state.maximumPrice]}
                        max={1000000}
                        min={1}
                        onChange={(values: any) => this.setState({ minimumPrice: values[0], maximumPrice: values[1] })} />
                </Form.Item>

                <Form.Item label="Type">
                    <Radio.Group defaultValue={this.adTypes[0]}
                        onChange={event => this.setState({ type: event.target.value })}
                        buttonStyle="solid">
                        {this.adTypes.map(x => (<Radio.Button value={x} key={x}>{x}</Radio.Button>))}
                    </Radio.Group>
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit">Apply</Button>
                </Form.Item>
            </Form>
        )
    }
}