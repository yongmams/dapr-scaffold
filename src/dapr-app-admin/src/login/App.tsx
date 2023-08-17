import { AuthProvider, AuthProviderProps, User } from 'oidc-react';
import LoggedIn from './LoggedIn';

const oidcConfig: AuthProviderProps = {
  onSignIn: async (userData: User | null) => {
    if (userData && userData.state && typeof userData.state === 'string') {
      window.location.replace(userData.state);
    } else {
      window.location.replace('/');
    }
  },
  authority: window.location.origin + '/sso',
  clientId: 'mvc',
  loadUserInfo: false
};

function App() {
  return (
    <AuthProvider {...oidcConfig}>
      <LoggedIn />
    </AuthProvider>
  )
}

export default App
