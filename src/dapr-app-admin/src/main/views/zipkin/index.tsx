import { connect } from 'react-redux';

const Zipkin: React.FC = () => {

    return (
        <iframe src="/zipkin" width="100%" height="100%" />
    );
}

const mapStateToProps = (state: any) => {
    return {
        ...state.app,
        ...state.settings
    }
}

export default connect(mapStateToProps)(Zipkin)