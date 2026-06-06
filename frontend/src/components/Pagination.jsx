export default function Pagination({
  page,
  totalPages,
  nextPage,
  prevPage,
  goToPage
}) {
  return (
    <div style={{ display: "flex", gap: 8, marginTop: 16, alignItems: "center" }}>
      
      <button onClick={prevPage} disabled={page === 1}>
        ◀
      </button>

      <span>
        Página {page} de {totalPages || 1}
      </span>

      <button onClick={nextPage} disabled={page === totalPages}>
        ▶
      </button>

      {/* salto rápido */}
      <input
        type="number"
        min="1"
        max={totalPages}
        placeholder="Ir a..."
        onKeyDown={(e) => {
          if (e.key === "Enter") {
            goToPage(Number(e.target.value));
          }
        }}
        style={{ width: 80 }}
      />
    </div>
  );
}