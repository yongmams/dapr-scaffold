import request from '@/utils/request';

export const info = async () =>
    request({
        url: '/api/user/info',
        method: 'post'
    });