
import { useEffect, useState } from 'react';
import { useAuth } from 'oidc-react';
import { HashRouter } from 'react-router-dom'
import { Provider } from 'react-redux'
import { ConfigProvider, Spin } from 'antd'

import zhCN from 'antd/es/locale/zh_CN'
import store from './store'
import Router from './router'

import './App.css'
import * as userApi from '@/api/user'

function App() {

  const auth = useAuth();
  const getAuthenticated = () => !!(auth && auth.userData);

  const [isAuthenticated, setAuthenticated] = useState(getAuthenticated());
  const [isLoading, setLoading] = useState(true);
  let isSend = false;

  useEffect(() => {
    setAuthenticated(getAuthenticated());
  }, [auth])

  useEffect(() => {
    if (!isSend && isAuthenticated) {
      userApi.info()
        .then(res => {
          console.log(res);
          isSend = true;
          setLoading(false);
        });
    }
  }, [isAuthenticated]);

  if (!isAuthenticated) {
    return <>请登录</>;
  }
  else if (isLoading) {
    return < Spin />;
  } else {
    return <>
      <ConfigProvider locale={zhCN}>
        <Provider store={store}>
          <HashRouter>
            <Router />
          </HashRouter>
        </Provider>
      </ConfigProvider>
    </>
  }
}

export default App
