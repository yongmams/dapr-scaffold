import React, { Suspense, useState } from 'react';
import {
    MenuFoldOutlined,
    MenuUnfoldOutlined,
    UploadOutlined,
    UserOutlined,
    VideoCameraOutlined,
} from '@ant-design/icons';
import { Layout, Menu, Button, theme } from 'antd';
import { MenuInfo, MenuClickEventHandler } from 'rc-menu/lib/interface';
import { connect } from 'react-redux';
import { Outlet, useNavigate } from 'react-router-dom';
import Loading from '@/components/Loading';

import './index.css';

const { Header, Sider, Content } = Layout;

const BaseLayout: React.FC = () => {
    const [collapsed, setCollapsed] = useState(false);
    const {
        token: { colorBgContainer },
    } = theme.useToken();

    const navigate = useNavigate();

    const handleClick: MenuClickEventHandler = (e: MenuInfo) => {
        console.log('click: ', e.key);

        if (!e.key) {
            return;
        } else if (e.key.startsWith('http')) {
            window.open(e.key, '_blank');
        } else {
            navigate(e.key);
        }
    };

    return (
        <Layout>
            <Sider trigger={null} collapsible collapsed={collapsed}>
                <div className="logo-vertical" />
                <Menu
                    theme="dark"
                    mode="inline"
                    defaultSelectedKeys={['1']}
                    onClick={handleClick}
                    items={[{
                        key: '/dashboard/index',
                        icon: <UserOutlined />,
                        label: 'Dashboard'
                    },
                    {
                        key: '2',
                        icon: <UserOutlined />,
                        label: '系统设置',
                        children: [
                            {
                                key: '/settings/zipkin',
                                icon: <UserOutlined />,
                                label: 'Zipkin',
                            },
                            {
                                key: '/settings/grafana',
                                icon: <UserOutlined />,
                                label: 'Grafana',
                            },
                            {
                                key: '/settings/kibana',
                                icon: <VideoCameraOutlined />,
                                label: 'Kibana',
                            },
                            {
                                key: '/settings/redis-insight',
                                icon: <UploadOutlined />,
                                label: 'Redis Insight',
                            },
                            {
                                key: '/settings/sqlpad',
                                icon: <UploadOutlined />,
                                label: 'SQLPad',
                            },
                            {
                                key: 'http://s3.sample.com',
                                icon: <UploadOutlined />,
                                label: 'MinIO Dashboard',
                            },
                            {
                                key: '/settings/webapp/upload',
                                icon: <UploadOutlined />,
                                label: 'WebApp',
                            }
                        ]
                    }]}
                />
            </Sider>
            <Layout>
                <Header style={{ padding: 0, background: colorBgContainer }}>
                    <Button
                        type="text"
                        icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
                        onClick={() => setCollapsed(!collapsed)}
                        style={{
                            fontSize: '16px',
                            width: 64,
                            height: 64,
                        }}
                    />
                </Header>
                <Content
                    style={{
                        margin: '12px 8px',
                        minHeight: 280,
                        background: colorBgContainer,
                    }}
                >
                    <Suspense fallback={<> <Loading /></>}>
                        <Outlet />
                    </Suspense>
                </Content>
            </Layout>
        </Layout>
    );
};

const mapStateToProps = (state: any) => {
    return {
        ...state.app,
        ...state.settings
    }
}

export default connect(mapStateToProps)(BaseLayout)