import { useEffect, useState } from "react";
import DynamicRouter from "./dynamicRouter";
import { asyncRoutes, constantRoutes, CustomRoute } from "./routes";
import { useLocation } from "react-router-dom";

const RouterComponent = () => {
    const { pathname } = useLocation();

    const [routeList, setRouteList] = useState<CustomRoute[]>([])
    let isUnmount = false

    const routeGuard = async () => {
        if (!isUnmount) {
            setRouteList(asyncRoutes.concat(constantRoutes));
        }
    };

    useEffect((): () => void => {
        routeGuard()
        return () => isUnmount = true
    }, [pathname])

    return (
        <>
            {routeList.length > 0 && <DynamicRouter routes={routeList} />}
        </>
    )
};

export default RouterComponent;