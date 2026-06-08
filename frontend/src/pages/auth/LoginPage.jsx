import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import "./LoginPage.css";

export default function LoginPage() {
  const navigate = useNavigate();
  const { user, login } = useAuth();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (loading) return;

    setError("");
    setLoading(true);

    try {
      await login(username, password);

      navigate("/dashboard", {
        replace: true,
      });
    } catch {
      setError("Usuario o contraseña incorrectos");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-page">
      <div className="login-card">

        {loading && (
          <div className="login-loading">
            <div className="loading-logo">
              💎
            </div>

            <h3 className="loading-title">
              SUMPTUOUS
            </h3>

            <div className="loading-dots">
              <span></span>
              <span></span>
              <span></span>
            </div>

            <p className="loading-message">
              Preparando tu espacio de trabajo
            </p>
          </div>
        )}

        <div className="login-header">
          <h1>SUMPTUOUS</h1>
          <p>Sistema de Facturación</p>
        </div>

        <form onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Usuario"
            value={username}
            disabled={loading}
            onChange={(e) => setUsername(e.target.value)}
          />

          <input
            type="password"
            placeholder="Contraseña"
            value={password}
            disabled={loading}
            onChange={(e) => setPassword(e.target.value)}
          />

          <button
            className="btn-primary"
            type="submit"
            disabled={loading}
          >
            {loading ? "Validando..." : "Entrar"}
          </button>
        </form>

        {error && (
          <p className="login-error">
            {error}
          </p>
        )}

      </div>
    </div>
  );
}