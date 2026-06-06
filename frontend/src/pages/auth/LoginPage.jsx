import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";

export default function LoginPage() {
  const navigate = useNavigate();
  const { login } = useAuth();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await login(username, password);
      navigate("/dashboard");
    } catch {
      setError("Usuario o contraseña incorrectos");
    }
  };

  return (
    <div className="page-container" style={{ maxWidth: 400, margin: "100px auto" }}>
      <h1>Iniciar Sesión</h1>

      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Usuario"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />

        <br /><br />

        <input
          type="password"
          placeholder="Contraseña"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <br /><br />

        <button className="btn-primary" type="submit">
          Entrar
        </button>
      </form>

      {error && <p>{error}</p>}
    </div>
  );
}