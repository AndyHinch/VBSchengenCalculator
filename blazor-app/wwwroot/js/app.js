window.downloadFile = (filename, content, mimeType = 'application/json') => {
    const blob = new Blob([content], { type: mimeType });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
};

window.printPage = () => window.print();

window.setTheme = (isDark) => {
    document.documentElement.setAttribute('data-theme', isDark ? 'dark' : 'light');
    localStorage.setItem('schengen_theme', isDark ? 'dark' : 'light');
};

window.getTheme = () => localStorage.getItem('schengen_theme') ?? 'light';
