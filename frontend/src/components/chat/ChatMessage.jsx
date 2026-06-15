export default function ChatMessage({
  text,
  isUser,
}) {
  return (
    <div
      className={`flex ${
        isUser
          ? "justify-end"
          : "justify-start"
      }`}
    >
      <div
        className={`
          max-w-[80%]
          rounded-xl
          px-4
          py-3
          whitespace-pre-wrap
          shadow-sm
          ${
            isUser
              ? "bg-blue-600 text-white"
              : "bg-white dark:bg-slate-800"
          }
        `}
      >
        {text}
      </div>
    </div>
  );
}