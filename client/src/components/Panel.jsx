export function Panel({ as: Component = "section", className = "", children, ...props }) {
  return (
    <Component className={`panel ${className}`.trim()} {...props}>
      {children}
    </Component>
  );
}
