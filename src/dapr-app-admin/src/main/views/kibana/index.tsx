import { connect } from 'react-redux';

const Kibana: React.FC = () => {

    return (
        <iframe src="/kb" width="100%" height="100%" />
    );
}

const mapStateToProps = (state: any) => {
    return {
        ...state.app,
        ...state.settings
    }
}

export default connect(mapStateToProps)(Kibana)