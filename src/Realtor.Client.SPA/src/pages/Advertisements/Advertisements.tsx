import React from 'react';
import './Advertisements.css';

import { Table, Button, Collapse } from 'antd';
import { Link } from 'react-router-dom';
import { searchAdvertisements } from '../../api/advertisement/methods';
import { SearchResponse, Advertisement, SearchOptions } from '../../api/advertisement/models';
import SearchFilters from '../../components/SearchFilters/SearchFilters';
import { connect } from 'react-redux';
const { Column } = Table;
const { Panel } = Collapse;

class Advertisements extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        this.state = {
            advertisements: [],
            isLoading: true,
            currentPage: 1,
            limit: 50,
            totalPages: 0,
            totalItems: 0,
            searchOptions: { minimumRooms: 1, maximumRooms: 100, minimumPrice: 100, maximumPrice: 1000000, type: "Ask" }
        };

        this.applySearchFilters = this.applySearchFilters.bind(this);
    }

    componentDidMount() {
        this.searchAdvertisements(this.state.currentPage, this.state.limit);
    }

    componentDidUpdate(prevProps: any, prevState: any) {
        if (prevState.searchOptions !== this.state.searchOptions)
            this.searchAdvertisements(1, this.state.limit)
    }

    async searchAdvertisements(page: number, limit: number) {
        this.setState({ isLoading: true })
        const results = await searchAdvertisements(page - 1, limit, this.state.searchOptions) as SearchResponse<Advertisement>
        this.setState({
            currentPage: results.currentPage + 1,
            totalPages: results.totalPages,
            totalItems: results.totalItems,
            advertisements: results.items,
            isLoading: false
        });
    }

    applySearchFilters(options: SearchOptions) {
        this.setState({ searchOptions: options });
    }

    render() {
        return (
            <div className="advertisements-layout-content">


                <div className="table-control-bar">{this.props.account.username ?
                    <Link to="/advertisement/create">
                        <Button type="primary" icon="form">Create</Button>
                    </Link> :
                    <Link to="/account/login">
                        <Button type="primary" icon="login">Login</Button>
                    </Link>}
                </div>

                <div className="table-filters-bar">
                    <Collapse bordered={false}>
                        <Panel header="Filters" key="1">
                            <SearchFilters onApply={this.applySearchFilters} />
                        </Panel>
                    </Collapse>
                </div>

                <Table dataSource={this.state.advertisements}
                    loading={this.state.isLoading}
                    style={{ marginTop: "8px" }}
                    rowKey="id"
                    pagination={{
                        onChange: (page, size) =>
                            this.searchAdvertisements(page, size as number)
                        ,
                        showTotal: (total, range) => `Total: ${total}`,
                        current: this.state.currentPage,
                        total: this.state.totalItems,
                        pageSize: this.state.limit
                    }}>
                    <Column title="Id" dataIndex="id" />
                    <Column title="Description" dataIndex="shortDescription" />
                    <Column title="Rooms" dataIndex="roomsCount" />
                    <Column title="Price" dataIndex="price" />
                    <Column title="Type" dataIndex="type" />
                    <Column
                        title="Action"
                        render={(text, record: Advertisement) => (
                            <span>
                                <Link to={`/advertisement/${record.id}`}>View</Link>
                            </span>
                        )}
                    />
                </Table>
            </div >);
    }
}

const mapStateToProps = (state: any) => {
    return {
        account: state.account
    }
};

export default connect(mapStateToProps)(Advertisements);