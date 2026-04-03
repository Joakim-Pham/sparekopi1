document.addEventListener('DOMContentLoaded', () => {

  // ── Mobile Nav ──
  const navToggle = document.querySelector('.nav-toggle');
  const navLinks  = document.querySelector('.nav-links');
  if (navToggle && navLinks) {
    navToggle.addEventListener('click', () => {
      navToggle.classList.toggle('open');
      navLinks.classList.toggle('open');
    });
    navLinks.querySelectorAll('a').forEach(link => {
      link.addEventListener('click', () => {
        navToggle.classList.remove('open');
        navLinks.classList.remove('open');
      });
    });
  }

  // ── Active nav link ──
  const currentPage = window.location.pathname.split('/').pop() || 'index.html';
  document.querySelectorAll('.nav-links a').forEach(link => {
    if (link.getAttribute('href') === currentPage) link.classList.add('active');
  });

  // ── Scroll fade-up ──
  const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry, i) => {
      if (entry.isIntersecting) {
        const siblings = entry.target.parentElement?.querySelectorAll('.fade-up') || [];
        let delay = 0;
        siblings.forEach((el, idx) => { if (el === entry.target) delay = idx * 80; });
        setTimeout(() => entry.target.classList.add('visible'), delay);
        observer.unobserve(entry.target);
      }
    });
  }, { threshold: 0.12 });
  document.querySelectorAll('.fade-up').forEach(el => observer.observe(el));

  // ── EmailJS contact form ──
  const EMAILJS_SERVICE_ID  = 'YOUR_SERVICE_ID';
  const EMAILJS_TEMPLATE_ID = 'YOUR_TEMPLATE_ID';

  const contactForm = document.getElementById('contactForm');
  const formSuccess = document.getElementById('formSuccess');

  if (contactForm) {
    contactForm.addEventListener('submit', async (e) => {
      e.preventDefault();
      const name    = contactForm.querySelector('[name="name"]')?.value.trim();
      const email   = contactForm.querySelector('[name="email"]')?.value.trim();
      const message = contactForm.querySelector('[name="message"]')?.value.trim();
      if (!name || !email || !message) {
        alert('Vennligst fyll inn alle obligatoriske felt.');
        return;
      }
      const btn = contactForm.querySelector('.form-submit');
      btn.textContent = 'Sender…';
      btn.disabled = true;
      try {
        await emailjs.sendForm(EMAILJS_SERVICE_ID, EMAILJS_TEMPLATE_ID, contactForm);
        contactForm.reset();
        formSuccess?.classList.add('visible');
        setTimeout(() => formSuccess?.classList.remove('visible'), 6000);
      } catch (err) {
        alert('Noe gikk galt. Ring oss på 22 11 47 55.');
      } finally {
        btn.textContent = 'Send melding →';
        btn.disabled = false;
      }
    });
  }

  // ── Animated print grid (homepage) ──
  const printGrid = document.querySelector('.print-grid');
  if (printGrid) {
    const cells = printGrid.querySelectorAll('.print-cell');
    setInterval(() => {
      const count = Math.floor(Math.random() * 4) + 1;
      for (let i = 0; i < count; i++) {
        const cell = cells[Math.floor(Math.random() * cells.length)];
        const roll = Math.random();
        cell.classList.remove('red', 'light');
        if (roll < 0.15) cell.classList.add('red');
        else if (roll < 0.35) cell.classList.add('light');
      }
    }, 600);
  }

  // ── Smooth scroll ──
  document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', (e) => {
      const target = document.querySelector(anchor.getAttribute('href'));
      if (target) {
        e.preventDefault();
        window.scrollTo({ top: target.getBoundingClientRect().top + window.scrollY - 80, behavior: 'smooth' });
      }
    });
  });

});
