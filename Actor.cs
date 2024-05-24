internal class Actor : MonoBehaviour
{
	public static List<Actor> AllActors = new List<Actor>();
	public int StartingHealth;
	int _currentHealth;
	private bool _isDead;
	private CancellationTokenSource _healthCancellationTokenSource;
	private CancellationTokenSource _deathCancellationTokenSource;

	public event Action<Actor> ActorDied;
	public event Action<Actor> ActorHealthChanged;

	public void TakeDamage(int damage)
	{
		_currentHealth -= damage;
		ActorHealthChanged?.Invoke(this);
		RecordHealthToServer();
		
		if (_currentHealth <= 0)
		{
			_isDead = true;
			ActorDied?.Invoke(this);
			RecordDeathToServer();
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
		_currentHealth = StartingHealth;
	}

	private void OnEnable()
	{
		AllActors.Add(this);
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
	}

	void RecordHealthToServer()
	{
		if (_healthCancellationTokenSource != null)
		{
			_healthCancellationTokenSource.Cancel();
		}

		_healthCancellationTokenSource = new CancellationTokenSource();
		Task.Run(() => RecordHealthToServerAsync(_healthCancellationTokenSource.Token));
	}
	
	Task RecordHealthToServerAsync(CancellationToken cancellationToken = default)
	{
		// this method makes a network call recording the health of the actor
		throw new NotImplementedException();
	}

	void RecordDeathToServer()
	{
		if (_deathCancellationTokenSource != null)
		{
			_deathCancellationTokenSource.Cancel();
			_deathCancellationTokenSource.Dispose();
		}

		_deathCancellationTokenSource = new CancellationTokenSource();
		Task.Run(() => RecordDeathToServer(_healthCancellationTokenSource.Token));
	}
	
	Task RecordDeathToServer(CancellationToken cancellationToken = default)
	{
		// this method makes a network call recording the death of the actor
		throw new NotImplementedException();
	}
	
	// network message from server received on background thread, triggering this callback
	void ReceiveHealthFromServer(int health)
	{
		_currentHealth = health;
		ActorHealthChanged?.Invoke(this);
	}
	
	// network message from server received on background thread, triggering this callback
	void ReceiveDeathFromServer(bool isDead)
	{
		_isDead = isDead;
		ActorDied?.Invoke(this);
	}
}
