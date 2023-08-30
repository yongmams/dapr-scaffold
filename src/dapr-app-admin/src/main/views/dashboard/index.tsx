import { connect } from "react-redux";

const Dashboard: React.FC = () => {
    return (
        <>
            aaaaa
        </>
    );
};

const mapStateToProps = (state: any) => {

    return {
        ...state.app,
        ...state.settings
    }
}

export default connect(mapStateToProps)(Dashboard)