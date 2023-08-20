import { To } from 'react-router-dom';
import Layout from '@/layouts'
import React from 'react';

export interface CustomRoute {
    title?: string;
    icon?: string;
    path?: string;
    hidden?: boolean;
    component?: any;
    redirect?: To;
    element?: JSX.Element;

    children?: CustomRoute[];
}

const constantRoutesList: CustomRoute[] = [
    // {
    //     path: '/401',
    //     // component : dynamicImport( () => import( /* webpackChunkName:'Error401'*/'@/views/errorPage/401' ) ),
    //     component: LazyLoad('views/errorPage/401'),
    //     hidden: true
    // },
    // {
    //     path: '/404',
    //     // component : dynamicImport( () => import( /* webpackChunkName:'Error401'*/'@/views/errorPage/404' ) ),
    //     component: React.lazy(() => import('@/views/errorPage/404'))LazyLoad('views/errorPage/404'),
    //     hidden: true
    // },
    {
        path: '/',
        redirect: '/dashboard/index'
    }
]

const asyncRoutesList: CustomRoute[] = [
    {
        path: '/dashboard',
        title: '首页',
        icon: 'home',
        redirect: '/dashboard/index',
        component: Layout,
        children: [
            {
                title: '首页',
                path: '/dashboard/index',
                hidden: true,
                component: React.lazy(() => import('@/views/dashboard'))
            }
        ]
    },
    {
        path: '/settings',
        title: '设置',
        icon: 'home',
        redirect: '/settings/index',
        component: Layout,
        children: [
            {
                title: '系统概览',
                path: '/settings/index',
                hidden: true,
                component: React.lazy(() => import('@/views/dashboard'))
            },
            {
                title: 'zipkin',
                path: '/settings/zipkin',
                hidden: true,
                component: React.lazy(() => import('@/views/zipkin'))
            } ,
            {
                title: 'ES',
                path: '/settings/kibana',
                hidden: true,
                component: React.lazy(() => import('@/views/kibana'))
            },
            {
                title: 'Redis',
                path: '/settings/redis-insight',
                hidden: true,
                component: React.lazy(() => import('@/views/redis-insight'))
            },
            {
                title: 'SQL',
                path: '/settings/sqlpad',
                hidden: true,
                component: React.lazy(() => import('@/views/sqlpad'))
            },
            {
                title: '文件上传',
                path: '/settings/webapp/upload',
                hidden: true,
                component: React.lazy(() => import('@/views/webapp/upload'))
            }
        ]
    },
];

export const asyncRoutes = asyncRoutesList
export const constantRoutes = constantRoutesList