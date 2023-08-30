export default [
    {
        url: '/api/user/info',
        method: 'post',
        timeout: 2000,
        response: (config) => {
            return {
                code: 200,
                msg: 'success',
                data: {}
            };
        }
    }
];
