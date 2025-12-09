import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAppSelector } from "../../store/hooks";

const ProtectedRoute = () => {
  const { token } = useAppSelector((state) => state.auth);
  const location = useLocation();

  if (!token) {
    // Redirecta till login, men spara "state.from" så vi kan skicka tillbaka dem sen
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
