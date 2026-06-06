import { Navigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";

export default function RoleRoute({
  children,
  roles,
}) {
  const { user } = useAuth();

  if (!user)
    return <Navigate to="/login" />;

  if (!roles.includes(user.rol))
    return <Navigate to="/dashboard" />;

  return children;
}