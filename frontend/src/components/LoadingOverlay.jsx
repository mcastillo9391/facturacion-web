import "./LoadingOverlay.css";

export default function LoadingOverlay({
  mensaje = "Procesando..."
}) {
  return (
    <div className="loading-overlay">

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
          {mensaje}
        </p>

      </div>

    </div>
  );
}