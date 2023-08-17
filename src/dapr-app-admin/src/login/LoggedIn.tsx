import { useAuth } from 'oidc-react';

const LoggedIn = () => {
    const auth = useAuth();
    if (auth && auth.userData && !auth.userData.state) {
        window.location.replace('/');
    }
    return <></>;
};

export default LoggedIn;