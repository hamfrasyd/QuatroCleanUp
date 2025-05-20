exports.AddTrashCollectedBatch = function(args) {
    var updates = args.Updates; // Array of { PlayerId, Amount }
    for (var i = 0; i < updates.length; i++) {
        var playerId = updates[i].PlayerId;
        var amount = updates[i].Amount;

        // Get current statistics
        var getStatsResult = server.GetPlayerStatistics({ PlayFabId: playerId });
        var stats = getStatsResult.Statistics;

        var daily = 0;
        var weekly = 0;
        var monthly = 0;

        if (stats) {
            var dailyStat = stats.find(s => s.StatisticName === "TotalTrashCollected_Daily");
            if (dailyStat) daily = dailyStat.Value;
            var weeklyStat = stats.find(s => s.StatisticName === "TotalTrashCollected_Weekly");
            if (weeklyStat) weekly = weeklyStat.Value;
            var monthlyStat = stats.find(s => s.StatisticName === "TotalTrashCollected_Monthly");
            if (monthlyStat) monthly = monthlyStat.Value;
        }

        // Update statistics
        var statUpdates = [
            { StatisticName: "TotalTrashCollected_Daily", Value: daily + amount },
            { StatisticName: "TotalTrashCollected_Weekly", Value: weekly + amount },
            { StatisticName: "TotalTrashCollected_Monthly", Value: monthly + amount }
        ];

        server.UpdatePlayerStatistics({ PlayFabId: playerId, Statistics: statUpdates });
    }
    return { success: true };
};