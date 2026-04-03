document.addEventListener('DOMContentLoaded', () => {

  // ── 1. Mobilmeny ──
  const navToggle = document.querySelector('.nav-toggle');
  const navLinks  = document.querySelector('.nav-links');
  if (navToggle && navLinks) {
    navToggle.addEventListener('click', () => {
      navToggle.classList.toggle('open');
      navLinks.classList.toggle('open');
    });
    navLinks.querySelectorAll('a').forEach(a => a.addEventListener('click', () => {
      navToggle.classList.remove('open');
      navLinks.classList.remove('open');
    }));
  }

  // ── 2. Aktiv nav-lenke ──
  const currentPage = window.location.pathname.split('/').pop() || 'index.html';
  document.querySelectorAll('.nav-links a').forEach(link => {
    if (link.getAttribute('href') === currentPage) link.classList.add('active');
  });

  // ── 3. Scroll fade-up animasjoner ──
  const observer = new IntersectionObserver(entries => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('visible');
        observer.unobserve(entry.target);
      }
    });
  }, { threshold: 0.1 });
  document.querySelectorAll('.fade-up').forEach(el => observer.observe(el));

  // ── 4. Kontaktskjema med EmailJS ──
  //
  // OPPSETT (5 min, gratis):
  // 1. Gå til https://emailjs.com og lag gratis konto
  // 2. Legg til Gmail-tjeneste → koble til Gmail-kontoen din
  // 3. Lag en e-postmal med disse variablene:
  //    {{from_name}}, {{from_email}}, {{phone}}, {{service}}, {{message}}
  // 4. Lim inn dine ID-er nedenfor:
  //
  const SERVICE_ID  = 'YOUR_SERVICE_ID';   // f.eks. 'service_abc123'
  const TEMPLATE_ID = 'YOUR_TEMPLATE_ID';  // f.eks. 'template_xyz789'
  // Public Key limes inn i HTML-filen (<script>emailjs.init(...)</script>)

  const form    = document.getElementById('contactForm');
  const success = document.getElementById('formSuccess');

  form?.addEventListener('submit', async e => {
    e.preventDefault();
    const name    = form.querySelector('[name="from_name"]')?.value.trim();
    const email   = form.querySelector('[name="from_email"]')?.value.trim();
    const message = form.querySelector('[name="message"]')?.value.trim();
    if (!name || !email || !message) {
      alert('Fyll inn navn, e-post og melding.');
      return;
    }
    const btn = form.querySelector('.form-submit');
    btn.textContent = 'Sender…';
    btn.disabled = true;
    try {
      await emailjs.sendForm(SERVICE_ID, TEMPLATE_ID, form);
      form.reset();
      if (success) { success.classList.add('visible'); setTimeout(() => success.classList.remove('visible'), 6000); }
    } catch {
      alert('Noe gikk galt. Ring oss på 22 11 47 55.');
    } finally {
      btn.textContent = 'Send melding →';
      btn.disabled = false;
    }
  });

  // ── 5. Smooth scroll ──
  document.querySelectorAll('a[href^="#"]').forEach(a => {
    a.addEventListener('click', e => {
      const target = document.querySelector(a.getAttribute('href'));
      if (target) {
        e.preventDefault();
        window.scrollTo({ top: target.getBoundingClientRect().top + scrollY - 72, behavior: 'smooth' });
      }
    });
  });

});
