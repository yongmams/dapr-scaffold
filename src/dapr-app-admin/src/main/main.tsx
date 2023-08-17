import React from 'react';
import ReactDOM from 'react-dom/client'
import { AuthProvider, AuthProviderProps } from 'oidc-react';
import App from './App.tsx'

import './index.css'

const oidcConfig: AuthProviderProps = {
  autoSignIn: true,
  onBeforeSignIn: () => {
    return window.location.href;
  },
  authority: window.location.origin + '/sso',
  clientId: 'mvc',
  redirectUri: window.location.origin + '/login.html',
  clientSecret: 'secret'
};

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <AuthProvider {...oidcConfig}>
      <App />
    </AuthProvider>
  </React.StrictMode>
)
