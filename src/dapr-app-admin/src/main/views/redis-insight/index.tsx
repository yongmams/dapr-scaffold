import { connect } from 'react-redux';

const RedisInsight: React.FC = () => {

    return (
        <iframe src="/redis" width="100%" height="100%" />
    );
}

const mapStateToProps = (state: any) => {
    return {
        ...state.app,
        ...state.settings
    }
}

export default connect(mapStateToProps)(RedisInsight)