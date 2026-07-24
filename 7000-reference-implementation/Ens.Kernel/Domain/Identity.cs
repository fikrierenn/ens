namespace Ens.Kernel.Domain;

// TRACE: ENS-4001 §Computational primitifler (Identity, deneysel — primitifliği kanıtlanmadı)
//
// Identity, zaman boyunca kimliği korunan, adreslenebilir bir referans birimidir.
// Bu implementasyon Identity'nin nihai ontolojik statüsüne (primitif mi, Event-kümelenmesinden
// türeyen bir projeksiyon mu — ENS-4001 v0.6 Design Review'da açık bırakıldı) karar vermez;
// yalnızca pratik/mühendislik ihtiyacını (aggregate-scoped consistency, event replay) karşılar.
public readonly record struct Identity(string Value)
{
    public static Identity New() => new(Guid.NewGuid().ToString("n"));

    public override string ToString() => Value;
}
