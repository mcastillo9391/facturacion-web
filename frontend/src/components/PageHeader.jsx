export default function PageHeader({
  title,
  subtitle,
  children
}) {
  return (
    <div className="page-header">
      <div>
        <h2>{title}</h2>
        {subtitle && (
          <p>{subtitle}</p>
        )}
      </div>

      <div>
        {children}
      </div>
    </div>
  );
}