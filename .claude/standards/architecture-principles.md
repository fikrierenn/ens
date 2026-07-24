---
id:            STD-ARCHITECTURE-PRINCIPLES
title:         Mimari İlkeler (Architecture Principles)
type:          standard
canon:         false
origin:        ENS-0000 §V, §VII, §VIII, §IX
depends_on:    [ENS-0000]
principles:    []
status:        ratified
owner:         ens-architect
version:       0.1.0
last_reviewed: 2026-07-24
---

# Mimari İlkeler (Architecture Principles)

**Yetki:** [ENS Anayasası, Madde V, VII, VIII, IX](../../0000-constitution/ENS-0000-constitution.md)
**Geçerli:** Faz 3'ten itibaren. Mimari teoriden **türetilir**, asla uydurulmaz.

## Temel direktif
Teori atfı olmayan hiçbir mimari karar var olamaz. Bir bileşenin gerçekleştirdiği teorik
kavramı adlandıramıyorsan, bileşen erkendir — Faz 1-2'ye dön. **Külliyat'ta olmayan bir
kavram mimaride tanıtılamaz** (Anayasa Madde IX).

## Türetme kuralı
Önerilen her bileşen için ADR'de yanıtla:
1. Bu, hangi theory kavramını / law'ı gerçekleştiriyor? (`realizes:`)
2. Hangi first principle'lara hizmet ediyor? (`principles:`)
3. Teori bu bileşenin ne yapması gerektiğini öngörüyor?
4. Alternatif nedir ve teori neden bunu tercih ediyor?

## Katmanlı referans modeli (Anayasa'nın sistem mimarisinden)
```
Layer 0  Enterprise Data Sources   (ERP, CRM, WMS, MES, mail, chat, docs, IoT, DB'ler)
Layer 1  Enterprise Knowledge Graph (her şey bağlı; izole kayıt yok)
Layer 2  Company Memory            (NE'yi değil NEDEN'i saklar)
Layer 3  Reasoning Engine          (hipotez, alternatif, confidence, açıklama)
Layer 4  Simulation Engine         (gerçeklikten önce what-if)
Layer 5  Decision Engine           (öneri, emir değil)
Layer 6  Learning Engine           (sonuçları ölçer, belleği günceller, reasoning'i iyileştirir)
```
Katmanlar bir akıl yürütme aracıdır, deployment topolojisi değil. Servis sınırlarını
katmanlar değil, bounded context'ler tanımlar.

## Bileşen nitelikleri (Anayasa Madde V)
Modular · Observable · Testable · Replaceable · Versioned · Explainable · mümkün olduğunda
Deterministic · Event-driven · DDD uyumlu · CQRS uyumlu · Cloud-native. Çatışmada
Explainable ve Testable kazanır.

## Sınırlar
- **Bounded context'ler** kendi model ve dillerine sahiptir; paylaşılan mutable tablolarla
  değil, yayımlanmış kontratlar ve domain event'lerle entegre olur.
- **Decision** event-sourced bir aggregate'tir; geçmişi audit trail'dir.
- **Mimariyi tek bir AI modeline/sağlayıcısına kilitleme** — model orchestration
  değiştirilebilir bir adapter'dır (modülerlik + Anayasa "mimariyi kilitleme").
- **Explainability yapısaldır:** açıklama nesnesi her öneriyle birlikte her sınırdan geçer;
  sonradan eklenen bir loglama değildir.

## Yapıt'lar (Faz 3 çıktıları, `5000-architecture/`)
C4 (context/container/component) · ADR'ler (`5000-architecture/adr/`) · Context Maps
(`5000-architecture/context-maps/`) · Event Model · Decision Graph · Memory Graph · Service
Boundaries. Diyagramlar Mermaid. Her yapıt izlenebilirlik header'ı taşır.
