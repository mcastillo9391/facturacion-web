import { createContext, useContext, useState } from "react";
import * as authService from "../services/authService";

const AuthContext = createContext();

export function AuthProvider({ children }) {
const [user, setUser] = useState(() => {
const savedUser = localStorage.getItem("user");

return savedUser
  ? JSON.parse(savedUser)
  : null;
});

const login = async (
username,
password
) => {
const result =
await authService.login(
username,
password
);

localStorage.setItem(
  "token",
  result.token
);

localStorage.setItem(
  "user",
  JSON.stringify(result)
);

setUser(result);
return result;
};

const logout = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("user");
  setUser(null);
};

return (
<AuthContext.Provider
value={{
user,
login,
logout,
isAuthenticated: !!user,
}}
>
{children}
</AuthContext.Provider>
);
}

export function useAuth() {
return useContext(AuthContext);
}
