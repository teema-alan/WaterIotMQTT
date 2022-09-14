namespace WaterIot.Enum
{
	public enum MqttBrokerConnectionStatus
	{
		DisConnected = 0,
		Connecting = 1,
		Connected = 2,
		Closed = 3,
		WaitToClose = 4,
		DisConnecting = 6
	}
}