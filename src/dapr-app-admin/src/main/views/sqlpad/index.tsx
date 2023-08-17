import { connect } from 'react-redux';

const SqlPad: React.FC = () => {

    return (
        <iframe src="/sqlpad" width="100%" height="100%" />
    );
}

const mapStateToProps = (state: any) => {
    return {
        ...state.app,
        ...state.settings
    }
}

export default connect(mapStateToProps)(SqlPad)