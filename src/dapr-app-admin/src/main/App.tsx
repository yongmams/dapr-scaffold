
import { useAuth } from 'oidc-react';
import { HashRouter } from 'react-router-dom'
import { Provider } from 'react-redux'
import { ConfigProvider } from 'antd'
import zhCN from 'antd/es/locale/zh_CN'
import store from './store'
import Router from './router'

import './App.css'

function App() {
  const auth = useAuth();

  return (
    <>
      {auth && auth.userData && <ConfigProvider locale={zhCN}>
        <Provider store={store}>
          <HashRouter>
            <Router />
          </HashRouter>
        </Provider>
      </ConfigProvider>}
    </>
  )
}

export default App
