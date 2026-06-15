const questions = [
  "¿Cuánto se ha pagado hoy?",
  "¿Cuál es la cartera total?",
  "¿Qué cliente debe más?",
  "¿Cuántos clientes hay?",
  "¿Cuánto se ha facturado este mes?",
  "¿Cuáles son los 5 clientes que más deben?",
];

export default function SuggestedQuestions({
  onSelect,
}) {
  return (
    <div className="flex flex-wrap gap-2">
      {questions.map((q) => (
        <button
          key={q}
          onClick={() => onSelect(q)}
          className="
            px-3
            py-2
            rounded-full
            border
            text-sm
            hover:bg-slate-100
            dark:hover:bg-slate-800
          "
        >
          {q}
        </button>
      ))}
    </div>
  );
}